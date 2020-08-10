/** @jsx jsx */
import { jsx } from "@emotion/core";
import { useSelector } from "react-redux";
import { useTheme } from "emotion-theming";
import { colors } from "./../../constants/colors";

export const TextDirection = ({ className, children, reverse, center }) => {
  const { ltr } = useSelector((state) => state.language);
  let align = ltr ? "text-left" : "text-right";
  if (reverse) {
    align = ltr ? "text-right" : "text-left";
  }
  if (center) {
    align = "text-center";
  }
  return <div className={`${className ? className : ""} ${align}`}>{children}</div>;
};

export const Paragraph = ({ className, text }) => {
  return (
    <div className={`text-center text-secondary ${className}`}>
      <p css={{ fontSize: "13px", lineHeight: "20px", padding: "0px 20px", margin: 0 }}>{text}</p>
    </div>
  );
};

export const SecondaryText = ({ className, children }) => {
  const theme = useTheme();
  return (
    <div
      className={className}
      css={{
        color: theme.secondaryTextColor,
        fontSize: "0.9rem",
        fontWeight: "bold",
      }}
    >
      {children}
    </div>
  );
};

export const PrimaryText = ({ className, gray, children }) => {
  const theme = useTheme();
  return (
    <span
      className={className}
      css={{
        color: gray ? colors.gray : theme.textColor,
      }}
    >
      {children}
    </span>
  );
};

export const Title = ({ className, text, gray, padding = "p-2" }) => {
  return (
    <div className={`${className ? className : ""}`}>
      {text ? (
        <div className={`${padding} font-weight-bold`}>
          <PrimaryText gray={gray}>
            {text}
          </PrimaryText>
        </div>
      ) : null}
    </div>
  );
};
