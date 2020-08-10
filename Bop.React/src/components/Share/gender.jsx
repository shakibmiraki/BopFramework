/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { ErrorMessage } from "formik";
import { useTranslation } from "react-i18next";
import { useFormikContext } from "formik";
import { SelectableItem } from "../Share/selectable-item";
import { gender } from "../../models/gender";
import { global_config } from "../../constants/constant";
import { images } from "../../constants/images";
import { Title, TextDirection } from "./text";

const styles = css({
  ".image-size": {
    margin: "auto",
    svg: {
      fontSize: "4rem",
      padding: "5px",
    },
  },
  ".col-6": {
    maxWidth: global_config.circle_fix_width,
    margin: "auto",
  },
});

export const GenderRadioWithError = () => {
  const gender_field_name = "gender";
  const formik = useFormikContext();
  const field = formik.getFieldProps(gender_field_name);
  const { t } = useTranslation();

  const isActive = (gender) => {
    return field.value === gender;
  };

  return (
    <div css={css(styles)}>
      <div className="col-8 m-auto">
        <TextDirection center>
          <Title text={t("share.text.gender")} />
        </TextDirection>
        <div className="row">
          <div className="col-6">
            <SelectableItem
              padding="p-2"
              className="rounded-circle img-rounded image-size"
              active={isActive(gender.woman)}
              onClick={() => formik.setFieldValue(gender_field_name, gender.woman)}
            >
              <img
                src={isActive(gender.woman) ? images.icons.woman_white : images.icons.woman}
                alt="woman"
                className="w-100"
              />
            </SelectableItem>
          </div>
          <div className="col-6">
            <SelectableItem
              padding="p-2"
              className="rounded-circle img-rounded image-size"
              active={isActive(gender.man)}
              onClick={() => formik.setFieldValue(gender_field_name, gender.man)}
            >
              <img
                src={isActive(gender.man) ? images.icons.man_white : images.icons.man}
                alt="man"
                className="w-100"
              />
            </SelectableItem>
          </div>
          <ErrorMessage name={gender_field_name} render={(msg) => <div className="text-danger">{t(msg)}</div>} />
        </div>
      </div>
    </div>
  );
};
