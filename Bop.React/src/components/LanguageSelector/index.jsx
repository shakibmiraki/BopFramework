/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { useTranslation } from "react-i18next";
import styles from "./index-style";
import { localeService } from "../../services/locale";
import { languages } from "../../constants/languages";
import { useSelector, useDispatch } from "react-redux";
import { setLanguage } from "./../../actions/language";
import { SecondaryText } from "./../Share/text";
import { OneColumnTickableItem } from "../Share/oneColumnTickableItem";

export const LanguageSelector = ({className=""}) => {
  const { t, i18n } = useTranslation();
  const { languageCode } = useSelector((state) => state.language);
  const dispatch = useDispatch();

  const isActive = (item) => {
    return languageCode === item.code;
  };

  return (
    <div className={className} css={css(styles)}>
      <SecondaryText className="text-center p-2">{t("language.title.choose_language")}</SecondaryText>

      <div className="row mb-2">
        {languages
          .sort((a, b) => b.order - a.order)
          .map(function (item, i) {
            return (
              <div className="col-6" key={item.code}>
                <OneColumnTickableItem
                  text={t(`share.language.${item.code}`)}
                  active={isActive(item)}
                  onClick={() => {
                    localeService.changeLanguage(i18n, item.code);
                    dispatch(setLanguage());
                  }}
                />
              </div>
            );
          })}
      </div>
    </div>
  );
};
