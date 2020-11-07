import { describe, test, expect } from "@jest/globals";
import originalTranslations from "@/assets/lang";

/**
 * Deeply remove values contained by an object
 *
 * @param obj Target
 */
const removeValues = (obj: object) => {
    if (!(obj instanceof Object)) {
        return;
    }

    for (const key in obj) {
        // @ts-ignore
        if (obj[key] instanceof Object) {
            // @ts-ignore
            removeValues(obj[key]);
        } else {
            // @ts-ignore
            obj[key] = undefined;
        }
    }
};



describe("i18n", () => {
    test("Assert contents", () => {
        // Remove values to test only keys
        const translations = JSON.parse(JSON.stringify(originalTranslations));
        removeValues(translations);

        for (const lang in translations) {
            for (const otherLang in translations) {
                // Skip comparison between the same language
                if (lang === otherLang) {
                    continue;
                }

                // @ts-ignore
                expect(translations[lang]).toStrictEqual(translations[otherLang]);
            }
        }
    });
});
