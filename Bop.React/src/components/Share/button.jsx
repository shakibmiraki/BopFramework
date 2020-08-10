/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { Line, ThemeBorder } from "./styled-component";
import { useTheme } from "emotion-theming";
import { ThemeColor } from "./color";

export const SolidButton = ({ backgroundColor, color, border, children, className, onClick, type }) => {
  const theme = useTheme();
  return (
    <button
      className={className}
      onClick={onClick}
      type={type}
      css={{
        padding: "9px 7px !important",
        backgroundColor: `${backgroundColor ? backgroundColor : theme.primaryColor} !important`,
        color: `${color ? color : theme.gradient[2]} !important`,
        border: `1px solid ${border ? border : theme.primaryColor} !important`,
      }}
    >
      {children}
    </button>
  );
};

export const GradientButton = ({ children, className }) => {
  const theme = useTheme();
  return (
    <button
      className={className}
      css={{
        padding: "9px 7px !important",
        backgroundImage: `linear-gradient(to right, ${theme.primaryColor} 0%, ${theme.gradient[0]} 40%, ${theme.gradient[1]} 100%)`,
        color: `${theme.gradient[2]} !important`,
      }}
    >
      {children}
    </button>
  );
};

export const Button = ({ type, text, onClick, icon }) => {
  return (
    <div>
      <SolidButton type={type} onClick={onClick} className={`btn btn-block rounded-pill`}>
        <span>{text}</span>
        <i css={css({ float: "right" })}>{icon}</i>
      </SolidButton>
    </div>
  );
};

export const StickyButton = ({ type, children, onClick, backgroundColor, color, border }) => {
  return (
    <div
      className="sticky-btn"
      css={css({
        marginBottom: "30px",
        marginTop: "30px",
      })}
    >
      <Line className="line" css={css({ position: "relative", top: "23px", zIndex: "-1" })} />
      <SolidButton
        type={type}
        onClick={onClick}
        className={`btn btn-block`}
        css={css({
          minWidth: "130px",
          borderTopLeftRadius: "30px",
          borderBottomLeftRadius: "30px",
          borderTopRightRadius: "0",
          borderBottomRightRadius: "0",
          float: "right",
          width: "auto",
          paddingLeft: "15px !important",
        })}
        backgroundColor={backgroundColor}
        color={color}
        border={border}
      >
        {children}
      </SolidButton>
    </div>
  );
};

export const BorderedButton = ({ className, onClick, icon, text }) => {
  return (
    <button type="button" className={`btn m-auto d-block ${className ? className : ""}`} onClick={onClick}>
      <ThemeColor>
        <ThemeBorder borderThick={2} borderStyle="solid" className="rounded-pill bg-white">
          <div className="p-2">
            {icon}
            <span css={css({ margin: "0 6px" })}>{text}</span>
          </div>
        </ThemeBorder>
      </ThemeColor>
    </button>
  );
};
