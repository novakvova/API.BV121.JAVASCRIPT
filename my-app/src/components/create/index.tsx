import { stat } from "fs";
import { ChangeEvent, useState } from "react";

interface IUserCreate {
  name: string;
  description: string;
}

const CreatePage = () => {
  const [state, setState] = useState<IUserCreate>({
    name: "",
    description: "",
  });

  const onChangeHandler= (e: ChangeEvent<HTMLInputElement> | ChangeEvent<HTMLTextAreaElement>) => {
    //console.log("Date input name", e.target.name);
    //console.log("Date input value", e.target.value);
    setState({...state, [e.target.name]: e.target.value});
  }

  const onSubmitHandler = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

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
              Image URL
            </label>
            <input
              type="text"
              className="form-control"
              id="image"
              name="image"
              placeholder="Enter image URL"
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
