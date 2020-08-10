/* eslint-disable react-hooks/exhaustive-deps */
/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { useTranslation } from "react-i18next";
import { Paragraph } from "./../Share/text";

const styles = css({});

export const NoTransaction = () => {
  const { t } = useTranslation();

  return (
    <div className="container" css={css(styles)}>
      <Paragraph text={t("transactions.paragraph.basic.no_transaction")} />
    </div>
  );
};
