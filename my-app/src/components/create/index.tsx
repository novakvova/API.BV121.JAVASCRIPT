import { ChangeEvent, useState } from "react";
import { useNavigate } from "react-router-dom";
import select from "../../assets/select.png";
import http from "../../http";

interface IUserCreate {
  name: string;
  description: string;
  image: File | null;
}

const CreatePage = () => {

  const navigator = useNavigate();

  const [state, setState] = useState<IUserCreate>({
    name: "",
    description: "",
    image: null
  });

  const onChangeHandler= (e: ChangeEvent<HTMLInputElement> | ChangeEvent<HTMLTextAreaElement>) => {
    //console.log("Date input name", e.target.name);
    //console.log("Date input value", e.target.value);
    setState({...state, [e.target.name]: e.target.value});
  }
  const onFileChangeHandler = (e: ChangeEvent<HTMLInputElement>) => {
    const {target} = e;
    const {files} = target;
    //e.target.files
    console.log("Show data ", files);
    if(files) {
      const file = files[0];
      setState({...state, image: file});
    }
    target.value="";
  }

  const onSubmitHandler = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    try {
      const result = await http.post("api/categories", state, {
        headers: {"Content-Type": "multipart/form-data"}
      });
      navigator("/");
    }
    catch(error: any) {
      console.log("Щось пішло не так", error);
      
    }
    console.log("Data send Server", state);
  };

  return (
    <>
      {/* <h1 className="text-center">Додати користувача</h1> */}

      <div className="row col-6 offset-3">
        <h1 className="mt-5 mb-4 text-center">Add User</h1>

        <form onSubmit={onSubmitHandler}>
          <div className="mb-3">
            <label htmlFor="name" className="form-label">
              Name
            </label>
            <input
              type="text"
              className="form-control"
              id="name"
              name="name"
              value={state.name}
              onChange={onChangeHandler}
              placeholder="Enter Name"
            />
            <div className="invalid-feedback">Please enter a valid name.</div>
          </div>

          <div className="mb-3">
            <label htmlFor="description" className="form-label">
              Description
            </label>
            <textarea
              className="form-control"
              id="description"
              name="description"
              value={state.description}
              onChange={onChangeHandler}
              rows={3}
              placeholder="Enter Description"
            ></textarea>
            <div className="invalid-feedback">
              Please enter a valid description.
            </div>
          </div>

          <div className="mb-3">
            <label htmlFor="image" className="form-label">
              <img
                src={(state.image==null ? select : URL.createObjectURL(state.image))}
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
            <div className="invalid-feedback">
              Please enter a valid image URL.
            </div>
          </div>

          <div className="text-center">
            <button type="submit" className="btn btn-primary">
              Add User
            </button>
          </div>
        </form>
      </div>
    </>
  );
};

export default CreatePage;
