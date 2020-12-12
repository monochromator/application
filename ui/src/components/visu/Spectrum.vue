<template>
  <div id="spectrum" />
</template>

<script lang="ts">
    import { Component, Vue, Prop } from "vue-property-decorator";
    import * as Plotly from "plotly.js";
/**
 * Data requiered to display the spectrum plot
 */
// interface SpectrumData {
//   spectrumData: ;
// }

@Component
    export default class Spectrum extends Vue {
  @Prop({ required: true })
  public spectrumData!: [number, number][];

  mounted() {
    const visibleColor: [number, string][] = [
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

    const colorscale: [number, string][] = visibleColor.map((c) => [
      c[0] / longMax,
      c[1]
    ]);

    const heatmapX: number[] = [];
    const zTemp: number[] = [];

    const start = 400;
    const end = 700;
    for (let i = start; i < end; i += 1) {
      zTemp.push(i);
      heatmapX.push(i);
    }
    const heatmapZ: number[][] = new Array(1).fill(zTemp);

    const heatmap: Plotly.Data = {
      x: heatmapX,
      z: heatmapZ,
      // zmin: longMin,
      // zmax: longMax,
      colorscale,
      type: "heatmap"
    };

    // Line plot
    const lineX: number[] = [];
    const lineY: number[] = [];

    this.spectrumData.forEach((specPoint: [number, number]) => {
      lineX.push(specPoint[0]);
      lineY.push(specPoint[1]);
    });

    const linePlot = {
      mode: "lines",
      x: lineX,
      y: lineY
    };

    console.log(lineX);
    console.log(lineY);

    Plotly.plot("spectrum", [ heatmap, linePlot ]);
  }
    }
</script>

<style scoped>
#spectrum {
  height: 100%;
}
</style>
