import './App.css';
import BookList from './components/Book/BookList';
import AddBook from './components/Book/AddBook';
import Container from 'react-bootstrap/Container'
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
  return (
    <Container>
      <BookList />
      <AddBook />
    </Container>   
  );
}

export default App;
