/* eslint-disable react-hooks/exhaustive-deps */
/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { TextDirection, Paragraph } from "./../../Share/text";
import { ThemeColor } from "../../Share/color";
import { AiOutlineCheck } from "react-icons/ai";
import { useFormikContext } from "formik";
import { transactionService } from "./../../../services/transaction";
import history from "./../../../services/history";

const styles = css({
  ".item:last-child .border-bottom": {
    borderBottom: "0 !important",
  },
});

export const TransactionsFilterType = () => {
  const { values, setFieldValue } = useFormikContext();
  const filter_type_fieldname = "filter_type";
  const isActive = (item) => {
    return values[filter_type_fieldname] === item.id;
  };

  const renderCheckIcon = (item) => {
    const display = isActive(item) ? "d-inline" : "d-none";
    return (
      <ThemeColor className={`${display}`}>
        <AiOutlineCheck />
      </ThemeColor>
    );
  };

  return (
    <div css={css(styles)}>
      <section className="bg-white shadow-sm pt-2 pb-2 mt-3">
        <div className="col-12 p-3">
          <Paragraph text="نوع تراکنش خود را انتخاب کنید"></Paragraph>
        </div>
        {transactionService.fetchTypeFilters()?.map(function (item, i) {
          return (
            <div
              className="col-12 item font-small-1 font-weight-bold"
              key={item.id}
              onClick={() => {
                setFieldValue(filter_type_fieldname, item.id);
                history.goBack();
              }}
            >
              <div className="row p-3 border-bottom">
                <div className="col-10">
                  <TextDirection>{item.name}</TextDirection>
                </div>
                <div className="col-2">{renderCheckIcon(item)}</div>
              </div>
            </div>
          );
        })}
      </section>
    </div>
  );
};
