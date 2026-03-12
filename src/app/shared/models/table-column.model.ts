import { TemplateRef } from '@angular/core';

export interface TableColumn<T> {
    id?: string;
    key?: keyof T;
    label: string;
    render?: (row: T) => string;
    template?: TemplateRef<{ $implicit: T }>;
}
