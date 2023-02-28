import React from 'react';
import logo from './logo.svg';
import './App.css';
import HomePage from './components/home';
import CreatePage from './components/create';
import { Route, Routes } from 'react-router-dom';
import Home from './components/home';

function App() {
  return (
    <>
      <div className="container">
        <Routes>
          <Route path="/">
            <Route index element={<Home />} />
            <Route path="users/create" element={<CreatePage />} />

            {/* <Route path="*" element={<NoMatch />} /> */}
          </Route>
        </Routes>
      </div>
    </>
  );
}

export default App;
