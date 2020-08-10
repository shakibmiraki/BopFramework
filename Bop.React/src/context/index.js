import React from "react";
import { useDispatch, useSelector } from "react-redux";
import { initLanguage } from "./../actions/language";
import { initTheme } from "./../actions/theme";
import { useEffect } from "react";
import { ThemeProvider } from "emotion-theming";
import { themeService } from "../services/theme";

const _init = (dispatch) => {
  //set language
  dispatch(initLanguage());

  //set theme
  dispatch(initTheme());
};

function AppProviders({ children }) {
  const dispatch = useDispatch();

  //it makes component re-render
  const { languageCode } = useSelector((state) => state.language);
  const { name } = useSelector((state) => state.theme);

  useEffect(() => _init(dispatch), [languageCode, name, dispatch]);

  return <ThemeProvider theme={themeService.getTheme()}>{children}</ThemeProvider>;
}

export default AppProviders;
