import React from 'react';
import logo from './logo.svg';
import './App.css';
import HomePage from './components/home';
import CreatePage from './components/create';

function App() {
  return (
    <>
      <div className="container">
        {/* <HomePage/> */}
        <CreatePage/>
      </div>
    </>
  );
}

export default App;
