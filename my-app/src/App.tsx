import React from 'react';
import logo from './logo.svg';
import './App.css';
import HomePage from './components/home';
import CreatePage from './components/create';
import { Route, Routes } from 'react-router-dom';
import Home from './components/home';
import LoginPage from './components/auth/login';
import DefaultLayout from './components/containers/default';

function App() {
  return (
    <>
      
        <Routes>
          <Route path="/" element={<DefaultLayout/>}>
            <Route index element={<Home />} />
            <Route path="users/create" element={<CreatePage />} />
            <Route path="account/login" element={<LoginPage />} />

            {/* <Route path="*" element={<NoMatch />} /> */}
          </Route>
        </Routes>
    </>
  );
}

export default App;
