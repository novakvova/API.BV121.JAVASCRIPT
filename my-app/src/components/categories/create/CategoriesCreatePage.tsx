import classNames from "classnames";
import { useFormik } from "formik";
import { ChangeEvent, useState } from "react";
import { useNavigate } from "react-router-dom";
import select from "../../../assets/select.png";
import http from "../../../http";
import InputGroup from "../../common/InputGroup";
import { ICategoryCreate } from "../types";
import { CreateSchema } from "./validation";

const CategoriesCreatePage = () => {
  const navigator = useNavigate();

  // const [state, setState] = useState<ICategoryCreate>({
  //   name: "",
  //   description: "",
  //   image: null
  // });

  const initValues: ICategoryCreate = {
    name: "",
    description: "",
    image: null,
  };

  // const onChangeHandler= (e: ChangeEvent<HTMLInputElement> | ChangeEvent<HTMLTextAreaElement>) => {
  //   setState({...state, [e.target.name]: e.target.value});
  // }


  // const onSubmitHandler = async (e: React.FormEvent<HTMLFormElement>) => {
  //   e.preventDefault();
  //   try {
  //     const result = await http.post("api/categories", state, {
  //       headers: {"Content-Type": "multipart/form-data"}
  //     });
  //     navigator("/");
  //   }
  //   catch(error: any) {
  //     console.log("Щось пішло не так", error);

  //   }
  //   console.log("Data send Server", state);
  // };

  const onSubmitFormik = async (values: ICategoryCreate) => {
    // console.log("Formik submit", values);
    try {
      const result = await http.post("api/categories", values, {
        headers: { "Content-Type": "multipart/form-data" },
      });
      navigator("/");
    } catch (error: any) {
      console.log("Щось пішло не так", error);
    }
  };

  const formik = useFormik({
    initialValues: initValues,
    validationSchema: CreateSchema,
    onSubmit: onSubmitFormik,
  });

  const { values, errors, touched, handleSubmit, handleChange, setFieldValue } = formik;

  const onFileChangeHandler = (e: ChangeEvent<HTMLInputElement>) => {
    const { target } = e;
    const { files } = target;
    //e.target.files
    console.log("Show data ", files);
    if (files) {
      const file = files[0];
      setFieldValue("image", file);
      //setState({...state, image: file});
    }
    target.value = "";
  };

  return (
    <>
      {/* <h1 className="text-center">Додати користувача</h1> */}

      <div className="row col-6 offset-3">
        <h1 className="mt-5 mb-4 text-center">Додати категорію</h1>

        <form onSubmit={handleSubmit}>

        <InputGroup
          field="name"
          label="Назва"
          onChange={handleChange}
          error={errors.name}
          touched={touched.name}
        />
          

          <div className="mb-3">
            <label htmlFor="description" className="form-label">
              Опис
            </label>
            <textarea
              className={classNames("form-control", {
                "is-invalid": touched["description"] && errors["description"],
                "is-valid": touched["description"] && !errors["description"],
              })}
              id="description"
              name="description"
              value={values.description}
              onChange={handleChange}
              rows={3}
              placeholder="Enter Description"
            ></textarea>
            {touched["description"] && errors["description"] && (
              <div className="invalid-feedback">{errors["description"]}</div>
            )}
          </div>

          <div className="mb-3">
            <label htmlFor="image" className="form-label">
              <img
                src={
                  values.image == null
                    ? select
                    : URL.createObjectURL(values.image)
                }
                alt="Оберіть фото"
                width="150px"
                style={{ cursor: "pointer" }}
              />
            </label>
            <input
              type="file"
              className="d-none"
              id="image"
              name="image"
              onChange={onFileChangeHandler}
            />
            {touched["image"] && errors["image"] && (
              <div className="alert alert-danger" role="alert">
                {errors["image"]}
              </div>
            )}
          </div>

          <div className="text-center">
            <button type="submit" className="btn btn-primary">
              Додати категорію
            </button>
          </div>
        </form>
      </div>
    </>
  );
};

export default CategoriesCreatePage;
