
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { APP_ENV } from "../../../../env";
import http from "../../../../http";

interface IUserItem {
    id: number,
    name: string,
    image: string,
    description: string
}
const AdminProductListPage = () => {
  //При зміни даних даного хука, компонент буде себе рендерить(буде себе оновлять)
  const [users, setUsers] = useState<Array<IUserItem>>([
    // {
    //   id: 1,
    //   name: "Соловейко",
    //   image:
    //     "https://as1.ftcdn.net/v2/jpg/02/80/79/88/1000_F_280798875_qUYbJvdurzbsM1Jm5JHdtdnhkuwOgoCN.jpg",
    //   description: "Дуже крутадівчина",
    // },
  ]);

  const content = users.map((user) => (
    <tr key={user.id}>
      <th scope="row">{user.id}</th>
      <td><img src={APP_ENV.IMAGE_PATH+user.image} alt="" width="150" /></td>
      <td>{user.name}</td>
      <td>{user.description}</td>
    </tr>
  ));

  // const onClickHandler = () => {
  //   //console.log("Click button");
  //   axios.get<Array<IUserItem>>("http://localhost:5059/api/users")
  //       .then((resp) => {
  //           console.log("Запит на сервер прийшов", resp);
  //           setUsers(resp.data);
  //       });
  //   //setUsers([]);
  // }

  useEffect(() => {
    http.get<Array<IUserItem>>("api/categories")
        .then((resp) => {
            console.log("Запит на сервер прийшов", resp);
            setUsers(resp.data);
        });
  }, []);
  

  return (
    <>
      <h1 className="text-center">Головна торінка</h1>

      <Link to="/categories/create" className="btn btn-danger"> 
        Додати категорію
      </Link>

      <Link to="/account/login" className="btn btn-success"> 
        Вхід
      </Link>

      {/* <button className="btn btn-success" onClick={onClickHandler}>Оновити список</button> */}

      <table className="table table-striped">
        <thead className="table-light">
          <tr>
            <th scope="col">Id</th>
            <th scope="col">Image</th>
            <th scope="col">Name</th>
            <th scope="col">Description</th>
          </tr>
        </thead>
        <tbody>
         {content}
        </tbody>
      </table>
    </>
  );
}
export default AdminProductListPage;