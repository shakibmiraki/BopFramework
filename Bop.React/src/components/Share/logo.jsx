/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { useTheme } from "emotion-theming";

export const StyledLogo = ({ className, children }) => {
  const theme = useTheme();
  return (
    <div
      className={className}
      css={{
        width: "100%",
        height: "20vmin",
        pointerEvents: "none",
        backgroundRepeat: "no-repeat",
        backgroundSize: "contain",
        backgroundPosition: "center",
        backgroundImage: `url(${theme.logo})`,
      }}
    >
      {children}
    </div>
  );
};

export const Logo = ({ className }) => {
  return (
    <div className={`text-center pb-3 pt-3 ${className}`}>
      <StyledLogo css={css({ width: "35px", height: "35px", margin: "auto" })} />
    </div>
  );
};
