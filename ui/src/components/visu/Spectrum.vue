<template>
  <div id="spectrum">
    <div id="btnList">
      <md-button class="md-raised" @click="close()"
        >{{ $t("spectrum.close_btn_title") }}
        <md-icon>close</md-icon></md-button
      >
      <md-button class="md-raised"
        >{{ $t("spectrum.save_btn_title") }} <md-icon>save</md-icon></md-button
      >

      <md-menu md-size="small">
        <md-button md-menu-trigger class="md-raised">{{
          $t("spectrum.filter_btn_title")
        }}</md-button>
        <md-menu-content>
          <md-menu-item>{{ $t("spectrum.filter_btn_raw") }}</md-menu-item>
          <md-menu-item>{{ $t("spectrum.filter_btn_smooth") }}</md-menu-item>
        </md-menu-content>
      </md-menu>
    </div>
    <div :id="'spectrum_plot_' + id" />
  </div>
</template>

<script lang="ts">
    import { Component, Vue, Prop, Watch } from "vue-property-decorator";
    import * as Plotly from "plotly.js";
    import { createLayout } from "@/common/Graph";

    const visibleColorSepctrum: [number, string][] = [
        [ 0, "#000000" ],
        [ 380, "#27005B" ], // Violet
        [ 449, "#2A007B" ], // Violet-bleu
        [ 466, "#002F83" ], // Bleu-violet
        [ 478, "#004769" ], // Bleu
        [ 483, "#005162" ], // Bleu-vert
        [ 490, "#00725F" ], // Vert-bleu
        [ 510, "#00AF6C" ], // Vert
        [ 541, "#59C000" ], // Vert-jaune
        [ 573, "#CAB300" ], // Jaune-vert
        [ 575, "#D2A900" ], // Jaune
        [ 579, "#D79300" ], // Jaune-orangé
        [ 584, "#DE8400" ], // Orangé-jaune
        [ 588, "#E77700" ], // Orangé
        [ 593, "#F55000" ], // Orangé-rouge
        [ 605, "#EA0021" ], // Rouge-orangé
        [ 622, "#7A0022" ], // Rouge
        [ 780, "#000000" ]
    ];

    const longMin = 0;
    const longMax = 780;

@Component
    export default class Spectrum extends Vue {
  @Prop({ required: true })
  public spectrumData: [number, number][];

  @Prop({ required: true })
  public id: string;

  @Prop({ required: true })
  public removeGraph: () => void;

  async mounted() {
    await this.initPlot();
    await this.updatePlot();
    await this.updateAxis();
  }

  async beforeUpdate() { await this.updateAxis(); }

  async initPlot() {
    // Draw plot
    await Plotly.react("spectrum_plot_" + this.id, [], createLayout(this), {
      displayModeBar: false,
      responsive: true
    });
    window.dispatchEvent(new Event("resize"));
  }

  @Watch("spectrumData")
  async onPropertyChanged() {
    // Props have changed, update the plot
    await this.updatePlot();
    await this.updateAxis();
  }

  /**
   * Draw a plot from the pros spectrumData
   */
  async updatePlot() {
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

    // Heatmap data creation
    const colorscale: [number, string][] = visibleColorSepctrum.map((c) => [
      c[0] / longMax,
      c[1]
    ]);

    const heatmapHeigth: number = (spectrumYMax - spectrumYMin) * 0.1;
    const heatmapX: number[] = [];
    const heatmapY: number[] = [
      spectrumYMin - heatmapHeigth * 1.1,
      spectrumYMin - heatmapHeigth * 0.1
    ]; // fill the top and the botom of the spectrum
    const zTemp: number[] = [];

    const heatmapStart = spectrumXMin;
    const heatmapEnd = spectrumXMax;

    for (let i = heatmapStart; i < heatmapEnd; i++) {
      heatmapX.push(i);
      if (i <= longMin) zTemp.push(longMin);
      else if (i >= longMax) zTemp.push(longMax);
      else zTemp.push(i);
    }

    zTemp[0] = longMin; // Replacement to plotly zmin
    zTemp[zTemp.length - 1] = longMax; // Replacement to plotly zmax

    const heatmapZ: number[][] = [ zTemp ];

    const linePlot: Plotly.Data = {
      name: "" + this.$t("spectrum.line_title"),
      mode: "lines",
      line: { color: "black", shape: "linear" },
      x: lineX,
      y: lineY
    };

    const heatmap: Plotly.Data = {
      name: "" + this.$t("spectrum.colorscale_title"),
      showscale: false,
      type: "heatmap",
      x: heatmapX,
      y: heatmapY,
      z: heatmapZ,
      colorscale
    };

    // Draw plot
    await Plotly.react("spectrum_plot_" + this.id, [ heatmap, linePlot ]);
  }

  async updateAxis() {
    await Plotly.relayout("spectrum_plot_" + this.id, createLayout(this));
  }

  /**
   * Close graph tab
   */
  close() {
    this.removeGraph();
  }
    }
</script>

<style scoped>
#spectrum {
  height: 100%;
  min-height: 40vh;
}
#btnList {
  display: flex;
  justify-content: flex-end;
  align-items: center;
}
</style>
