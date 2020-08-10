import i18n from "i18next";
import LanguageDetector from "i18next-browser-languagedetector";
import XHR from "i18next-xhr-backend";

//translate json files
import en from "./locales/en/translation.json";
import fa from "./locales/fa/translation.json";

i18n
  .use(XHR)
  .use(LanguageDetector)
  .init({
    debug: false,
    fallbackLng: "en",
    keySeparator: false,
    interpolation: {
      escapeValue: false,
    },
    resources: {
      en: { translations: en },
      fa: { translations: fa },
    },
    detection: {
      lookupLocalStorage: "i18nextLng",
      order: ["localStorage"],
    },
    ns: ["translations"],
    defaultNS: "translations",
  });

export default i18n;
