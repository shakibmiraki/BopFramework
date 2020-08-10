import { toast } from "react-toastify";

const notify = (message) => message && toast.info(message);
const notifies = (messages) => messages && messages.forEach((message) => notify(message));

export const toastService = {
  notify,
  notifies,
};
