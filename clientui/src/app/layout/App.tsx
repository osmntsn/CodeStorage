import React from 'react';
import { Route, Routes } from 'react-router-dom';
import Dashboard from '../../features/Dashboard/Dashboard';
import './App.css';
import AppBar from './AppBar';

function App() {
  return (
    <div className="App">
      <header>
        <link
          rel="stylesheet"
          href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap"
        />
        <link
          rel="stylesheet"
          href="https://fonts.googleapis.com/icon?family=Material+Icons"
        />
      </header>
      <AppBar/>
      <Routes>
        <Route path="/" element={<Dashboard />} />
      </Routes>
    </div>
  );
}

export default App;
