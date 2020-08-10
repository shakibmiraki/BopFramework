/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { colors } from "./../../constants/colors";
import { ThemeBackgroundColor } from "../Share/color";

const styles = css({
  "& .active": {
    transition: "all 0.3s",
  },
  "& .active *": {
    color: `${colors.white} !important`,
  },
  "& .selectable": {
    padding: 0,
    overflow: "hidden",
  },
  // "& .selectable *": {
  //   fontWeight: "500 !important",
  //   transition: "background-color 0.3s",
  // },
});

export const SelectableItem = ({ children, onClick, active, rounded, className,padding }) => {
  return (
    <div css={css(styles)} onClick={onClick}>
      <div
        className={`text-center mt-2 mb-2 selectable border shadow-sm bg-white d-block rounded 
          ${active ? "active" : ""}
          ${rounded ? "rounded-pill" : ""}
          ${className}
          `}
      >
        <ThemeBackgroundColor className={padding} backgroundColor={active ? null : colors.white}>
          {children}
        </ThemeBackgroundColor>
      </div>
    </div>
  );
};
