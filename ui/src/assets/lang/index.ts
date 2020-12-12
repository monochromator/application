import merge from "deepmerge";
import { LocaleMessages } from "vue-i18n";

import languages from "./languages.json";
import startup from "./startup.json";

// DO NOT FORGET TO ADD YOUR NEW LANGUAGE FILE TO MERGE PARAMETERS

export default merge.all([ languages, startup ]) as LocaleMessages;
