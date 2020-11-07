import merge from "deepmerge";

import languages from "./languages.json";

// DO NOT FORGET TO ADD YOUR NEW LANGUAGE FILE TO MERGE PARAMETERS

export default merge.all([ languages ]);
