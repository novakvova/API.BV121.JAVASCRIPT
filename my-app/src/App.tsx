import React from 'react';
import logo from './logo.svg';
import './App.css';
import HomePage from './components/home';
import { Route, Routes } from 'react-router-dom';
import Home from './components/home';
import LoginPage from './components/auth/login';
import DefaultLayout from './components/containers/default';
import CategoriesCreatePage from './components/categories/create';
import ResetPasswordPage from './components/auth/resetpassword';

function App() {
  return (
    <>
      
        <Routes>
          <Route path="/" element={<DefaultLayout/>}>
            <Route index element={<Home />} />
            <Route path="categories/create" element={<CategoriesCreatePage />} />
            <Route path="account/login" element={<LoginPage />} />
            <Route path="resetpassword" element={<ResetPasswordPage />} />

            {/* <Route path="*" element={<NoMatch />} /> */}
          </Route>
        </Routes>
    </>
  );
}

export default App;
