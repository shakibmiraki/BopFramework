import * as types from "../actionTypes/theme";
import { themeService } from "./../services/theme";
import { themes } from "./../constants/theme";


export const setActiveTheme = () => ({
  type: types.SetActiveTheme,
  payload: themeService.getThemeName() ?? themeService.setThemeName(themes.maroon.name),
});

export const initTheme = () => (dispatch) => {
  dispatch(setActiveTheme());
};
