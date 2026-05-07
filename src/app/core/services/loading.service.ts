import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class LoadingService {
    private activeRequests = 0;
    private loadingSubject = new BehaviorSubject<boolean>(false);

    get loading$(): Observable<boolean> {
        return this.loadingSubject.asObservable();
    }

    startLoading(): void {
        this.activeRequests += 1;
        if (this.activeRequests === 1) {
            this.loadingSubject.next(true);
        }
    }

    stopLoading(): void {
        if (this.activeRequests > 0) {
            this.activeRequests -= 1;
        }
        if (this.activeRequests === 0) {
            this.loadingSubject.next(false);
        }
    }
}
