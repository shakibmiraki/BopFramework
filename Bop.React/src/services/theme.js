import { themes } from "./../constants/theme";
import { localeService } from "./locale";
import { localStorageService } from "./localStorage";
import { storage_key } from "../constants/constant";
import { config } from "./../config";

const getThemeName = () => localStorageService.getKey(storage_key.theme_name);

const getTheme = () => {
  let name = getThemeName();
  if (name) {
    return themes[name];
  } else {
    themeService.setThemeName(config.defaultTheme);
    name = themeService.getThemeName();
    return themes[name];
  }
};

const getFlexRowClass = () => {
  let className = "flex-row-reverse";
  if (localeService.isLTR()) {
    className = "flex-row";
  }
  return className;
};

const setThemeName = (themeName) => {
  localStorageService.setKey(storage_key.theme_name, themeName);
};

const setThemeInitialized = () => {
  localStorageService.setKey(storage_key.theme_initialize, true);
};

export const themeService = {
  getThemeName,
  getTheme,
  getFlexRowClass,
  setThemeName,
  setThemeInitialized,
};
