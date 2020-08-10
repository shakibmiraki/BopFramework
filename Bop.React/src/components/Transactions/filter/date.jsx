/* eslint-disable react-hooks/exhaustive-deps */
/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { FaAngleLeft } from "react-icons/fa";
import { Collapse } from "react-collapse";
import { useState } from "react";
import { PATCalendar } from "./../../Share/calendar";
import { useFormikContext } from "formik";
import { dateService } from "../../../services/date";
import { Paragraph, PrimaryText } from "../../Share/text";

const styles = css({
  ".date": {
    fontSize: "0.9rem",
    fontWeight: "bold",
  },
});

export const TransactionsFilterDate = () => {
  const [fromDateIsOpen, setFromDateIsOpen] = useState(false);
  const [toDateIsOpen, setToDateIsOpen] = useState(false);
  const { values } = useFormikContext();

  return (
    <div css={css(styles)}>
      <div className="bg-white border-bottom text-center pt-1 pb-1">
        <div className="col-12 p-3">
          <Paragraph text="بازه زمانی مورد نظر خود را انتخاب کنید"></Paragraph>
        </div>
        <div className="col-12">
          <div className="row p-3 date" onClick={(e) => setFromDateIsOpen(!fromDateIsOpen)}>
            <div className="col-3">
              <PrimaryText>از تاریخ</PrimaryText>
            </div>
            <div className="col-7">
              <PrimaryText>{dateService.jsonToString(values.filter_fromdate)}</PrimaryText>
            </div>

            <div className="col-2">
              <FaAngleLeft />
            </div>
          </div>
        </div>
        <div className="col-12">
          <Collapse isOpened={fromDateIsOpen}>
            <PATCalendar name="filter_fromdate" onDateSelect={() => setFromDateIsOpen(!fromDateIsOpen)} />
          </Collapse>
        </div>
      </div>

      <div className="bg-white border-bottom text-center pt-1 pb-1">
        <div className="col-12">
          <div className="row p-3 date" onClick={(e) => setToDateIsOpen(!toDateIsOpen)}>
            <div className="col-3">
              <PrimaryText>تا تاریخ</PrimaryText>
            </div>
            <div className="col-7">
              <PrimaryText>{dateService.jsonToString(values.filter_todate)}</PrimaryText>
            </div>
            <div className="col-2">
              <FaAngleLeft />
            </div>
          </div>
        </div>
        <div className="col-12">
          <Collapse isOpened={toDateIsOpen}>
            <PATCalendar name="filter_todate" onDateSelect={() => setToDateIsOpen(!toDateIsOpen)} />
          </Collapse>
        </div>
      </div>
    </div>
  );
};
