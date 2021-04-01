import './App.css';
import Navbar from './components/Navbar/Navbar';
import BookList from './components/Book/BookList';

function App() {
  return (
    <div >
      <Navbar />
      <div class="mx-48">
        <BookList />
      </div>     
    </div>    
  );
}

export default App;
