import React from 'react'
import axios from 'axios'

export default class BookList extends React.Component {
    state = {
        books: []
    }

    // Get Request
    componentDidMount() {
        axios.get(`https://favelidabookstoreapi.azurewebsites.net/api/Book/Fiction`)
        .then(res => {
            const books = res.data;
            this.setState({ books })
        })
    }

    render() {
        return(
            <div class="flex flex-col">
                <table className="min-w-full divide-y divide-gray-200">
                    <thead>
                        <tr>
                            <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                Name
                            </th>
                            <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                Category
                            </th>
                            <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                Author
                            </th>
                            <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                Price
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.state.books.map(book => {
                            return (
                                <tr key="{book}">
                                    <td>{book.bookName}</td>
                                    <td>{book.category}</td>
                                    <td>{book.author}</td>
                                    <td>{book.price}</td>
                                </tr>
                            )
                        })}
                    </tbody>
                </table>
            </div>           
            
        );
    }
}