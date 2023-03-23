import { useDispatch, useSelector } from "react-redux";
import { Link, useNavigate } from "react-router-dom";
import { AuthActionType, IAuthUser } from "../../auth/types";

const DefaultHeader = () => {

  const {isAuth} = useSelector((store: any)=> store.auth as IAuthUser);
  const dispatch = useDispatch();

  const handleLogout = () => {
    dispatch({type: AuthActionType.USER_LOGOUT});
  }



  return (
    <>
      <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
        <div className="container">
          <Link className="navbar-brand" to="/">
            My App
          </Link>

          <button
            className="navbar-toggler"
            type="button"
            data-bs-toggle="collapse"
            data-bs-target="#navbarNav"
            aria-controls="navbarNav"
            aria-expanded="false"
            aria-label="Toggle navigation"
          >
            <span className="navbar-toggler-icon"></span>
          </button>

          <div className="collapse navbar-collapse" id="navbarNav">
            <ul className="navbar-nav me-auto mb-2 mb-lg-0">
              <li className="nav-item">
                <Link to="/" className="nav-link active" aria-current="page">
                  Home
                </Link>
              </li>
              <li className="nav-item">
                <Link to="categories/create" className="nav-link">
                  Додати категорію
                </Link>
              </li>
              <li className="nav-item">
                <Link to="products/list" className="nav-link">
                  Продукти
                </Link>
              </li>
            </ul>
            <ul className="navbar-nav ms-auto mb-2 mb-lg-0">
              {isAuth ? (
                <>
                <li className="nav-item">
                  <button
                    onClick={handleLogout}
                    className="nav-link btn btn-link"
                  >
                    Log Out
                  </button>
                </li>
                 <li className="nav-item">
                 <Link to="/admin" className="nav-link">
                   Admin
                 </Link>
               </li>
               </>
              ) : (
                <>
                  <li className="nav-item">
                    <Link to="account/register" className="nav-link">
                      Register
                    </Link>
                  </li>
                  <li className="nav-item">
                    <Link to="account/login" className="nav-link">
                      Log In
                    </Link>
                  </li>
                </>
              )}
            </ul>
          </div>
        </div>
      </nav>
    </>
  );
};
export default DefaultHeader;
