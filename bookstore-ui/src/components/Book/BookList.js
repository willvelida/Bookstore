import React from 'react';
import axios from 'axios';
import Table from 'react-bootstrap/Table';

export default class BookList extends React.Component {
    state = {
        books: []
    }

    componentDidMount() {
        axios.get(`https://favelidabookstoreapi.azurewebsites.net/api/Books`)
        .then(res => {
            const books = res.data;
            this.setState({ books })
        })
    }

    render() {
        return(
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Category</th>
                        <th>Author</th>
                        <th>Price</th>
                    </tr>
                </thead>
                <tbody>
                    {this.state.books.map(book => {
                        return (
                            <tr key={book}>
                                <td>
                                    {book.bookName}
                                </td>
                                <td>
                                    {book.category}
                                </td>
                                <td>
                                    {book.author}
                                </td>
                                <td>
                                    {book.price}
                                </td>
                            </tr>
                        )
                    })}
                </tbody>
            </Table>
        )
    }
}