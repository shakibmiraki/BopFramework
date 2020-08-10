/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { useFormikContext } from "formik";
import { ThemeBackgroundColor } from "./color";
import { colors } from "../../constants/colors";

const styles = css({
  /* The switch - the box around the slider */
  ".switch": {
    position: "relative",
    display: "inline-block",
    width: "54px",
    height: "26px",
    margin: 0,
  },
  /* Hide default HTML checkbox */
  ".switch input": {
    opacity: 0,
    width: 0,
    height: 0,
  },
  /* The slider */
  ".slider": {
    position: "absolute",
    cursor: "pointer",
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
    backgroundColor: "#ccc",
    transition: ".4s",
    borderRadius: "34px",
    overflow: "hidden",
  },
  ".slider:before": {
    position: "absolute",
    content: '" "',
    height: "18px",
    width: "20px",
    left: "3px",
    bottom: "4px",
    backgroundColor: "white",
    transition: ".4s",
    borderRadius: "50%",
    zIndex: "1",
  },
  "input:focus + .slider": {
    boxShadow: "0 0 1px #2196F3",
  },
  "input:checked + .slider:before": {
    transform: "translateX(26px)",
    zIndex: "1",
  },
  ".slider .background": {
    position: "absolute",
    top: "0",
    left: "0",
    right: "0",
    bottom: "0",
  },
});

export const SwitchButton = ({ fieldname }) => {
  const formik = useFormikContext();
  const field = formik.getFieldProps(fieldname);

  return (
    <div css={css(styles)}>
      <label className="switch">
        <input
          name={fieldname}
          type="checkbox"
          checked={field.value}
          onChange={(e) => formik.setFieldValue(fieldname, e.target.checked)}
        />

        <span className="slider round">
          <ThemeBackgroundColor
            backgroundColor={field.value === true ? null : colors.gray_light_2}
            className="background"
          ></ThemeBackgroundColor>
        </span>
      </label>
    </div>
  );
};
