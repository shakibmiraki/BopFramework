/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { PrimaryText } from "./text";
import { IoMdArrowRoundBack } from "react-icons/io";
import { Navigation } from "./../Menu/navigation";
import { ThemeColor } from "./color";
import history from "./../../services/history";
import { SquaredBackground, CoverImage } from "./image";
import { Logo } from "./logo";

export const Back = () => {
  return (
    <span
      css={css({ position: "absolute", left: "15px", top: "13px", fontSize: "32px", zIndex: 1 })}
      onClick={() => {
        history.goBack();
      }}
    >
      <ThemeColor>
        <IoMdArrowRoundBack css={css({ float: "right" })} />
      </ThemeColor>
    </span>
  );
};

export const Header = ({ logo, burger, back, text, height = "150", cover }) => {
  return (
    <div css={css({ position: "relative" })}>
      <div css={css({ height: `${height}px` })}>{cover ? <CoverImage url={cover} /> : <SquaredBackground />}</div>
      <div css={css({ position: "absolute", left: "0", right: "0", top: "0" })}>
        {burger ? <Navigation /> : null}
        {logo ? <Logo /> : null}
        {text ? (
          <div css={css({ position: "absolute", fontSize: "32px", width: "100%", textAlign: "center" })}>
            <PrimaryText gray className="font-small-1 font-weight-bold">
              {text}
            </PrimaryText>
          </div>
        ) : null}
        {back ? <Back /> : null}
      </div>
    </div>
  );
};
