/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { useTranslation } from "react-i18next";
import styles from "./index-style";
import { useDispatch } from "react-redux";
import { themeService } from "../../services/theme";
import { setActiveTheme } from "./../../actions/theme";
import { themes } from "./../../constants/theme";
import { SelectableItem } from "./../Share/selectable-item";
import { SecondaryText } from "../Share/text";
import { GrReactjs } from "react-icons/gr";

export const ThemeSelector = () => {
  const { t } = useTranslation();
  const dispatch = useDispatch();
  const activeTheme = themeService.getThemeName();

  const isActive = (themeName) => {
    return activeTheme === themeName;
  };

  return (
    <div css={css(styles)}>
      <SecondaryText className="text-center p-2">{t("language.title.choose_theme")}</SecondaryText>

      <div className="theme-selector">
        <div className="row mb-2">
          {Object.keys(themes).map(function (key) {
            return (
              <div className="col-4 max-width" key={themes[key].name}>
                <SelectableItem
                  padding="p-2"
                  className="rounded-circle img-rounded image-size"
                  active={isActive(themes[key].name)}
                  onClick={() => {
                    themeService.setThemeName(themes[key].name);
                    dispatch(setActiveTheme());
                  }}
                >
                  <GrReactjs color={themes[key].primaryColor} />
                </SelectableItem>
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
};
