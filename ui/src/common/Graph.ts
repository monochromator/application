import Vue from "vue";
import * as Plotly from "plotly.js";

/**
 * Create graph layout
 *
 * @param component Graph component
 * @return {Partial<Plotly.Layout>} Layout
 */
const createLayout = (component: Vue): Partial<Plotly.Layout> => {
    return {
        xaxis: {
            title: {
                text: "" + component.$t("spectrum.xAxis_title")
            }
        },
        yaxis: {
            title: {
                // Todo : look for the axis unit
                text: "" + component.$t("spectrum.yAxis_title")
            }
        },
        margin: {
            l: 50,
            r: 20,
            b: 50,
            t: 0
        }
    };
};

export { createLayout };
