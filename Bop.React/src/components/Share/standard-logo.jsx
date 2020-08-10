/* eslint-disable react-hooks/exhaustive-deps */
/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { images } from "./../../constants/images";

const styles = css({
  img: {
    maxWidth: "60px",
  },
});

export const StandardLogo = () => {
  return (
    <div css={css(styles)}>
      <div className="row standard-logos justify-content-center">
        <div className="col-3 p-1 text-center">
          <img className="p-2" src={images.icons.irankish} alt="shaparak logo" />
        </div>
        <div className="col-3 p-1 text-center">
          <img className="p-2" src={images.icons.shaparak} alt="irankish logo" />
        </div>
      </div>
    </div>
  );
};
