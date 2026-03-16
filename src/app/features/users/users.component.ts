import { Component } from '@angular/core';
import { TableColumn } from '../../shared/models/table-column.model';
import { SharedModule } from '../../shared/shared.module';

interface User {
    id: number;
    name: string;
    email: string;
    role: string;
}

@Component({
    selector: 'app-users',
    templateUrl: './users.component.html',
    styleUrls: ['./users.component.scss'],
    standalone: true,
    imports: [SharedModule]
})
export class UsersComponent {
    users: User[] = [
        { id: 1, name: 'Alice Nguyen', email: 'alice@example.com', role: 'admin' },
        { id: 2, name: 'Bob Tran', email: 'bob@example.com', role: 'manager' },
        { id: 3, name: 'Charlie Pham', email: 'charlie@example.com', role: 'user' }
    ];

    columns: TableColumn<User>[] = [
        { key: 'id', label: 'ID' },
        { key: 'name', label: 'Name' },
        { key: 'email', label: 'Email' },
        { key: 'role', label: 'Role' }
    ];
}
