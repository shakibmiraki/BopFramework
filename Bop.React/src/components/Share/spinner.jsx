/** @jsx jsx */
import { jsx } from "@emotion/core";
import { FaSpinner } from "react-icons/fa";
import { keyframes } from "@emotion/core";
import { useSelector } from "react-redux";

export const Spinner = (props) => {
  return (
    <FaSpinner
      css={{
        animation: `${keyframes({
          "0%": { transform: "rotate(0deg)" },
          "100%": { transform: "rotate(360deg)" },
        })} 1s linear infinite`,
      }}
      aria-label="loading"
      {...props}
    />
  );
};

export const FullPageSpinner = () => {
  const { isRequesting } = useSelector((state) => state.request);
  return (
    <div
      css={{
        position: "fixed",
        width: "100%",
        height: "100%",
        display: "flex",
        justifyContent: "center",
        fontSize: "3rem",
        top: 0,
        left: 0,
        right: 0,
        bottom: 0,
        backgroundColor: "#00000038",
        opacity: isRequesting ? 1 : 0,
        zIndex: isRequesting ? "9999999" : "-1",
      }}
    >
      <div css={{ alignSelf: "center", color: "#3e3e3e" }}>
        <Spinner />
      </div>
    </div>
  );
};
