import merge from "deepmerge";
import { LocaleMessages } from "vue-i18n";

import languages from "./languages.json";
import startup from "./startup.json";
import homeContentToolBar from "./home.content.toolbar.json";
import analysisDialog from "./analysis_dialog.json";
import spectrum from "./spectrum.json";
import homeContent from "./home.content.json";
import toolbar from "./toolbar.json";
import common from "./common.json";
import calibrationDialog from "./calibration_dialog.json";

// DO NOT FORGET TO ADD YOUR NEW LANGUAGE FILE TO MERGE PARAMETERS

export default merge.all([
    languages,
    startup,
    spectrum,
    homeContentToolBar,
    analysisDialog,
    homeContent,
    toolbar,
    common,
    calibrationDialog
]) as LocaleMessages;
