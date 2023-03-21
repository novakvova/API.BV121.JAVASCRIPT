import * as yup from "yup";

export const CreateSchema = yup.object({
  name: yup.string().required("Поле не повинне бути пустим"),
  description: yup.string().required("Поле не повинне бути пустим"),
  image: yup.mixed().required("Оберіть будь-ласка файл"),
});
