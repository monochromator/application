<template>
  <div id="spectrum">
    <div id="btnList">
      <md-button class="md-raised">{{$t("spectrum.save_btn_title")}} <md-icon>save</md-icon></md-button>
        <md-menu md-size="small">
      <md-button md-menu-trigger class="md-raised">{{$t("spectrum.filter_btn_title")}}</md-button>

      <md-menu-content>
        <md-menu-item>{{$t("spectrum.filter_btn_raw")}}</md-menu-item>
        <md-menu-item>{{$t("spectrum.filter_btn_smooth")}}</md-menu-item>
      </md-menu-content>
    </md-menu>
    </div>
    <div id="spectrum_plot"/>
  </div>
</template>

<script lang="ts">
    import { Component, Vue, Prop } from "vue-property-decorator";
    import * as Plotly from "plotly.js";

    // Todo : set the plotId to be randomly generated to avoid plot id concurency

@Component
    export default class Spectrum extends Vue {
  @Prop({ required: true })
  public spectrumData!: [number, number][];

  mounted() {
    const visibleColorSepctrum: [number, string][] = [
      [ 0, "#000000" ],
      [ 380, "#27005B" ],
      [ 449, "#2A007B" ],
      [ 466, "#002F83" ],
      [ 478, "#004769" ],
      [ 483, "#005162" ],
      [ 490, "#00725F" ],
      [ 510, "#00AF6C" ],
      [ 541, "#59C000" ],
      [ 573, "#CAB300" ],
      [ 575, "#D2A900" ],
      [ 579, "#D79300" ],
      [ 584, "#DE8400" ],
      [ 588, "#E77700" ],
      [ 593, "#F55000" ],
      [ 605, "#EA0021" ],
      [ 622, "#7A0022" ],
      [ 780, "#000000" ]
    ];

    const longMax = 780;

    // Line plot data creation
    const lineX: number[] = [];
    const lineY: number[] = [];

    this.spectrumData.forEach((specPoint: [number, number]) => {
      lineX.push(specPoint[0]);
      lineY.push(specPoint[1]);
    });

    const spectrumYMin: number = Math.min(...lineY);
    const spectrumYMax: number = Math.max(...lineY);

    const spectrumXMin: number = Math.min(...lineX);
    const spectrumXMax: number = Math.max(...lineX);

    const linePlot: Plotly.Data = {
      name: "" + this.$t("spectrum.line_title"),
      mode: "lines",
      line: {
        color: "black",
        shape: "spline"
      },
      x: lineX,
      y: lineY
    };

    // Heatmap data creation
    const colorscale: [number, string][] = visibleColorSepctrum.map((c) => [
      c[0] / longMax,
      c[1]
    ]);

    const heatmapHeigth: number = (spectrumYMax - spectrumYMin) * 0.1;
    const heatmapX: number[] = [];
    const heatmapY: number[] = [ spectrumYMin - heatmapHeigth * 1.1, spectrumYMin - heatmapHeigth * 0.1 ]; // fill the top and the botom of the spectrum
    const zTemp: number[] = [];

    const heatmapStart = spectrumXMin;
    const heatmapEnd = spectrumXMax;

    for (let i = heatmapStart; i < heatmapEnd; i++) {
      zTemp.push(i);
      heatmapX.push(i);
    }
    zTemp[0] = 0; // To
    const heatmapZ: number[][] = [ zTemp ];

    const heatmap: Plotly.Data = {
      name: "" + this.$t("spectrum.colorscale_title"),
      x: heatmapX,
      y: heatmapY,
      z: heatmapZ,
      colorscale,
      showscale: false,
      type: "heatmap"
    };

    // Draw plot
    Plotly.plot("spectrum_plot", [ heatmap, linePlot ], {
          xaxis: {
            title: {
              text: "" + this.$t("spectrum.xAxis_title")
            }
          },
          yaxis: {
            title: {
              // Todo : look for the axis unit
              text: "" + this.$t("spectrum.yAxis_title")
            }
          },
                  margin: {
          l: 50,
          r: 20,
          b: 50,
          t: 0
        }

    }, {
        displayModeBar: false,
        responsive: true
      });
  }
    }
</script>

<style scoped>
#spectrum {
  height: 100%;
}
#btnList{
  display: flex;
  justify-content: flex-end;
  align-items: center;
}
#spectrum_plot{
  height: 100%;
}
</style>
