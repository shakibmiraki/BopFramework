/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { useSelector } from "react-redux";
import { colors } from "../../constants/colors";
import { Dashed } from "../Share/dashed";
import { ImgUpload } from "./../Share/image-upload";
import { TextDirection } from "../Share/text";

const styles = css({
  padding: "10px 0",
  h6: {
    color: colors.gray,
  },
});

export const ProfileInfo = () => {
  const { profile } = useSelector((state) => state.profile);

  return (
    <div css={css(styles)}>
      <div className="profile-info">
        <div className="media position-relative align-items-center">
          {/* <img className="rounded-circle mr-3 ml-3 border bg-white" src={images.avatar} alt="..." /> */}

          <ImgUpload />

          <TextDirection>
            <div className="media-body">
              <h6 className="mt-0">{`${profile.firstName} ${profile.lastName}`}</h6>
            </div>
          </TextDirection>
        </div>
      </div>
      <div className="p-3">
        <Dashed borderThick={1} />
      </div>
    </div>
  );
};
