import { useState } from "react";
import { useSearchParams } from "react-router-dom";

interface ForgotPasswrodForm {
    userId: string|null,
    token: string|null,
    password: string;
    confirmPassword: string;
  }

const ResetPasswordPage = () => {
    let [searchParams] = useSearchParams();

    const [state, setState] = useState<ForgotPasswrodForm>({
        userId: searchParams.get("userId"),
        token: searchParams.get("code"),
        password: "",
        confirmPassword: ""
      });
      console.log("Model ", state);

    return (
      <>
        <div className="container col-6 offset-3">
          <h1 className="mt-2 mb-3 text-center">Вкажіть новий пароль</h1>
        </div>
      </>
    );
}

export default ResetPasswordPage;