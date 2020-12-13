import merge from "deepmerge";
import { LocaleMessages } from "vue-i18n";

import languages from "./languages.json";
import startup from "./startup.json";
import homeContentToolBar from "./home.content.toolbar.json";
import analysisDialog from "./analysis_dialog.json";

// DO NOT FORGET TO ADD YOUR NEW LANGUAGE FILE TO MERGE PARAMETERS

export default merge.all([ languages, startup, homeContentToolBar, analysisDialog ]) as LocaleMessages;
