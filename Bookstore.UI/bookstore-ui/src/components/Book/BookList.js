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
                <div className="-my-2 overflow-x-auto sm:-mx-6 lg:-mx-8">
                    <div className="py-2 align-middle inline-block min-w-full sm:px-6 lg:px-8">
                        <div className="shadow overflow-hidden border-b border-gray-200 sm:rounded-lg">
                            <table className="min-w-full divide-y divide-gray-200">
                                <thead className="bg-gray-50">
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
                                        <th scope="col" class="relative px-6 py-3">
                                            <span class="sr-only">Edit</span>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody className="bg-white divide-y divide-gray-200">
                                    {this.state.books.map(book => {
                                        return (
                                            <tr key="{book}">
                                                <td className="px-6 py-4 whitespace-nowrap">
                                                    {book.bookName}
                                                </td>
                                                <td className="px-6 py-4 whitespace-nowrap">
                                                    {book.category}
                                                </td>
                                                <td className="px-6 py-4 whitespace-nowrap">
                                                    {book.author}
                                                </td>
                                                <td className="px-6 py-4 whitespace-nowrap">
                                                    {book.price}
                                                </td>
                                                <td className="px-6 py-4 whitespace-nowrap">
                                                    <a href="#" class="text-indigo-600 hover:text-indigo-900">View</a>
                                                    <br />
                                                    <a href="#" class="text-indigo-600 hover:text-indigo-900">Edit</a>
                                                </td>
                                            </tr>
                                        )
                                    })}
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>           
            
        );
    }
}