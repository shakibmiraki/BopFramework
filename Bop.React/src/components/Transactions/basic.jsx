/* eslint-disable react-hooks/exhaustive-deps */
/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { useTranslation } from "react-i18next";
import { useSelector } from "react-redux";
import { NoTransaction } from "./no-transaction";
import { TransactionList } from "./transaction-list";
import { Title, TextDirection } from "../Share/text";

const styles = css({});

export const TransactionsBasic = () => {
  const { t } = useTranslation();
  const { profile } = useSelector((state) => state.profile);
  return (
    <div className="container" css={css(styles)}>
      <TextDirection center>
        <Title text={t("transations.basic.title")} />
      </TextDirection>
      {profile?.transactions?.length > 0 ? <TransactionList /> : <NoTransaction />}
    </div>
  );
};
