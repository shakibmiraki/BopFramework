/* eslint-disable react-hooks/exhaustive-deps */
/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { useTranslation } from "react-i18next";
import { useSelector } from "react-redux";
import { TextDirection, Paragraph, SecondaryText } from "./../Share/text";
import { AiOutlineSearch } from "react-icons/ai";
import { IoMdOptions } from "react-icons/io";
import { FaCreditCard } from "react-icons/fa";
import { FcCheckmark } from "react-icons/fc";
import { dateService } from "../../services/date";
import history from "./../../services/history";
import { routes } from "./../../constants/constant";
import _ from "lodash";
import { transactionService } from "./../../services/transaction";

const styles = css({
  ".list-icon": {
    svg: {
      margin: "0.3rem",
      fontSize: "1.3rem",
    },
  },
  ".transactions": {
    "> .row": {
      borderRadius: "1rem !important",
    },
  },
});

export const TransactionList = () => {
  const { t } = useTranslation();
  const { profile } = useSelector((state) => state.profile);
  console.log(profile.transactions);

  return (
    <div className="container" css={css(styles)}>
      <Paragraph text={t("manage_contact.paragraph.contact_list.select_number")} />

      <div className="d-flex flex-row-reverse list-icon">
        <AiOutlineSearch />
        <IoMdOptions onClick={() => history.push(`${routes.transactions.base}${routes.transactions.filter}`)} />
      </div>
      <div className="transactions mt-3">
        {_.orderBy(profile.transactions, "timestamp", ["desc"]).map(function (transaction, index) {
          return (
            <div className="row shadow-sm bg-white rounded p-3 mb-3 border" key={index}>
              <div className="col-12">
                <div className="row">
                  <div className="col-2">
                    <FaCreditCard />
                  </div>
                  <div className="col-6">
                    <TextDirection>
                      <SecondaryText>
                        {transactionService.transactionName(transaction.transactionType)}
                      </SecondaryText>
                    </TextDirection>
                  </div>
                  <div className="col-4">
                    <TextDirection reverse>
                      <SecondaryText>{dateService.unixToJalaliString(transaction.timestamp)}</SecondaryText>
                    </TextDirection>
                  </div>
                </div>
              </div>
              <div className="col-12">
                <div className="row">
                  <div className="col-2">
                    <FcCheckmark />
                  </div>
                  <div className="col-10">
                    <TextDirection reverse>
                      {new Intl.NumberFormat("fa", {
                        style: "currency",
                        currency: "IRR",
                        minimumFractionDigits: 0,
                      }).format(transaction.amount)}
                    </TextDirection>
                  </div>
                </div>
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
};
