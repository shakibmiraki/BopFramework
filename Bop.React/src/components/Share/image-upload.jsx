/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { BsPerson } from "react-icons/bs";
import { useState } from "react";
import { profileService } from "../../services/profile";
import { useEffect } from "react";

const styles = css({
  ".field": {
    margin: "5px",
    display: "flex",
    flexDirection: "column",
  },
  "input[type='file']": {
    display: "none",
  },
  ".custom-file-upload": {
    display: "inline-block",
    position: "relative",
    padding: "6px",
    cursor: "pointer",
  },
  ".img-wrap": {
    position: "relative",
    width: "90px",
    height: "90px",
    overflow: "hidden",
    border: "1px solid #dadada",
  },
  img: {
    height: "100%",
    width: "100%",
  },
  svg: {
    fontSize: "5rem",
    width: "100%",
    height: "100%",
    padding: "10px",
  },
});

export const ImgUpload = () => {
  const [avatar, setAvatar] = useState("");

  const photoUpload = (e) => {
    e.preventDefault();
    const reader = new FileReader();
    const file = e.target.files[0];
    reader.onloadend = () => {
      setAvatar(reader.result);
      profileService.setAvatar(reader.result);
    };
    if (file) {
      reader.readAsDataURL(file);
    }
  };

  useEffect(() => {
    const avatar = profileService.getAvatar();
    if (avatar) {
      setAvatar(avatar);
    }
  }, []);

  return (
    <div css={css(styles)}>
      <label htmlFor="photo-upload" className="custom-file-upload fas rounded-pill m-0">
        <div className="img-wrap img-upload rounded-pill">
          {avatar ? <img htmlFor="photo-upload" src={avatar} alt="" /> : <BsPerson />}
        </div>
        <input id="photo-upload" type="file" onChange={(e) => photoUpload(e)} />
      </label>
    </div>
  );
};
