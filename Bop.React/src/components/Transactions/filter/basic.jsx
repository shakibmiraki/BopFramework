/* eslint-disable react-hooks/exhaustive-deps */
/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { useTranslation } from "react-i18next";
import { TextDirection, SecondaryText, PrimaryText, Title } from "./../../Share/text";
import { FaAngleLeft, FaRegCalendarAlt, FaRegMoneyBillAlt, FaRegCheckSquare } from "react-icons/fa";
import { RiCheckboxMultipleBlankLine } from "react-icons/ri";
import { ThemeColor } from "../../Share/color";
import { AiOutlineCheck } from "react-icons/ai";
import history from "./../../../services/history";
import { routes } from "./../../../constants/constant";
import { useFormikContext } from "formik";
import { dateService } from "../../../services/date";
import { transactionService } from "../../../services/transaction";

const styles = css({});

export const TransactionsFilterBasic = () => {
  const { t } = useTranslation();
  const { values, setFieldValue } = useFormikContext();
  const filter_sortby_fieldname = "sortby";

  const isActive = (item) => {
    return values[filter_sortby_fieldname] === item;
  };

  const renderCheckIcon = (item) => {
    const opacity = isActive(item) ? "1" : "0";
    return (
      <ThemeColor css={css({opacity : opacity})}>
        <AiOutlineCheck />
      </ThemeColor>
    );
  };

  return (
    <div css={css(styles)}>
      <section className="bg-white shadow-sm pt-2 pb-2">
        <TextDirection className="col-12" center>
          <Title text={t("transations.title.filter.filter")} gray />
        </TextDirection>
        <div className="col-12">
          <div
            className="row p-3 border-bottom"
            onClick={() =>
              history.push(`${routes.transactions.base}${routes.transactions.filter}${routes.transactions.date}`)
            }
          >
            <div className="col-2 d-flex justify-content-center align-items-center">
              <FaRegCalendarAlt />
            </div>
            <div className="col-10">
              <div className="row">
                <div className="col-10">
                  <div className="row">
                    <TextDirection className="col-12 font-weight-bold">
                      <div className="font-small-1">تاریخ</div>
                      {values.filter_fromdate && values.filter_todate ? (
                        <PrimaryText className="font-small-2">
                          از {dateService.jsonToString(values.filter_fromdate)} تا{" "}
                          {dateService.jsonToString(values.filter_todate)}
                        </PrimaryText>
                      ) : (
                        <SecondaryText className="font-small-2">تاریخ مورد نظر خود را انتخاب کنید</SecondaryText>
                      )}
                    </TextDirection>
                  </div>
                </div>
                <div className="col-2 d-flex justify-content-center align-items-center">
                  <FaAngleLeft />
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-12">
          <div
            className="row p-3 border-bottom"
            onClick={() =>
              history.push(`${routes.transactions.base}${routes.transactions.filter}${routes.transactions.amount}`)
            }
          >
            <div className="col-2 d-flex justify-content-center align-items-center">
              <FaRegMoneyBillAlt />
            </div>
            <div className="col-10">
              <div className="row">
                <div className="col-10">
                  <div className="row">
                    <TextDirection className="col-12 font-weight-bold">
                      <div className="font-small-1">مبلغ</div>
                      {values.filter_amount ? (
                        <PrimaryText className="font-small-2">
                          {
                            transactionService
                              .fetchAmountFilters()
                              ?.filter((t) => t.id === values.filter_amount)[0].name
                          }
                        </PrimaryText>
                      ) : (
                        <SecondaryText className="font-small-2">نمایش همه مبالغ تراکنش شده</SecondaryText>
                      )}
                    </TextDirection>
                  </div>
                </div>
                <div className="col-2 d-flex justify-content-center align-items-center">
                  <FaAngleLeft />
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-12">
          <div
            className="row p-3 border-bottom"
            onClick={() =>
              history.push(`${routes.transactions.base}${routes.transactions.filter}${routes.transactions.status}`)
            }
          >
            <div className="col-2 d-flex justify-content-center align-items-center">
              <FaRegCheckSquare />
            </div>
            <div className="col-10">
              <div className="row">
                <div className="col-10">
                  <div className="row">
                    <TextDirection className="col-12 font-weight-bold">
                      <div className="font-small-1">وضعیت تراکنش</div>
                      {values.filter_status ? (
                        <PrimaryText className="font-small-2">
                          {
                            transactionService
                              .fetchStatusFilters()
                              ?.filter((t) => t.id === values.filter_status)[0].name
                          }
                        </PrimaryText>
                      ) : (
                        <SecondaryText className="font-small-2">نمایش وضعیت همه تراکنش ها</SecondaryText>
                      )}
                    </TextDirection>
                  </div>
                </div>
                <div className="col-2 d-flex justify-content-center align-items-center">
                  <FaAngleLeft />
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-12">
          <div
            className="row p-3"
            onClick={() =>
              history.push(`${routes.transactions.base}${routes.transactions.filter}${routes.transactions.type}`)
            }
          >
            <div className="col-2 d-flex justify-content-center align-items-center">
              <RiCheckboxMultipleBlankLine />
            </div>
            <div className="col-10">
              <div className="row">
                <div className="col-10">
                  <div className="row">
                    <TextDirection className="col-12 font-weight-bold">
                      <div className="font-small-1">نوع تراکنش</div>
                      <SecondaryText className="font-small-2"></SecondaryText>
                      {values.filter_type ? (
                        <PrimaryText className="font-small-2">
                          {
                            transactionService.fetchTypeFilters()?.filter((t) => t.id === values.filter_type)[0]
                              .name
                          }
                        </PrimaryText>
                      ) : (
                        <SecondaryText className="font-small-2">نمایش همه تراکنش ها</SecondaryText>
                      )}
                    </TextDirection>
                  </div>
                </div>
                <div className="col-2 d-flex justify-content-center align-items-center">
                  <FaAngleLeft />
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      <section className="bg-white shadow-sm pt-2 pb-2 mt-3">
        <TextDirection className="col-12" center>
          <Title text={t("transations.title.filter.sort")} gray />
        </TextDirection>

        <div className="col-12" onClick={() => setFieldValue(filter_sortby_fieldname, "date")}>
          <div className="row p-3 border-bottom">
            <div className="col-2 d-flex justify-content-center align-items-center">
              <FaRegCalendarAlt />
            </div>
            <div className="col-10">
              <div className="row">
                <div className="col-10">
                  <TextDirection className="font-weight-bold font-small-1">تاریخ</TextDirection>
                </div>
                <div className="col-2">{renderCheckIcon("date")}</div>
              </div>
            </div>
          </div>
        </div>

        <div className="col-12" onClick={() => setFieldValue(filter_sortby_fieldname, "amount")}>
          <div className="row p-3 border-bottom">
            <div className="col-2 d-flex justify-content-center align-items-center">
              <FaRegMoneyBillAlt />
            </div>
            <div className="col-10">
              <div className="row">
                <div className="col-10">
                  <TextDirection className="font-weight-bold font-small-1">مبلغ</TextDirection>
                </div>
                <div className="col-2">{renderCheckIcon("amount")}</div>
              </div>
            </div>
          </div>
        </div>

        <div className="col-12" onClick={() => setFieldValue(filter_sortby_fieldname, "status")}>
          <div className="row p-3">
            <div className="col-2 d-flex justify-content-center align-items-center">
              <FaRegCheckSquare />
            </div>
            <div className="col-10">
              <div className="row">
                <div className="col-10">
                  <TextDirection className="font-weight-bold font-small-1">وضعیت تراکنش</TextDirection>
                </div>
                <div className="col-2">{renderCheckIcon("status")}</div>
              </div>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
};
