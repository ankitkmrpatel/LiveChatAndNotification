import React from "react";

export default function LoginPageLoadingComponent() {
  return (
    <div className="flex flex-col md:flex-row flex-1 bg-blue-950">
      <main className="flex-1">
        <section className="bg-gray-50 dark:bg-gray-900">
          <div className="flex flex-col items-center justify-center px-6 py-8 mx-auto md:h-screen lg:py-0">
            <div
              role="status"
              className="max-w-md p-4 border border-gray-200 rounded shadow animate-pulse md:p-6 dark:border-gray-700"
            >
              <div className="h-2.5 bg-gray-200 rounded-full dark:bg-gray-700 w-32 mb-2.5"></div>
              <div className="w-48 h-2 mb-10 bg-gray-200 rounded-full dark:bg-gray-700"></div>
              <div className="flex items-baseline mt-4 space-x-6">
                <div className="w-full bg-gray-200 rounded-t-lg h-72 dark:bg-gray-700"></div>
                <div className="w-full h-56 bg-gray-200 rounded-t-lg dark:bg-gray-700"></div>
                <div className="w-full bg-gray-200 rounded-t-lg h-72 dark:bg-gray-700"></div>
                <div className="w-full h-64 bg-gray-200 rounded-t-lg dark:bg-gray-700"></div>
                <div className="w-full bg-gray-200 rounded-t-lg h-80 dark:bg-gray-700"></div>
                <div className="w-full bg-gray-200 rounded-t-lg h-72 dark:bg-gray-700"></div>
                <div className="w-full bg-gray-200 rounded-t-lg h-80 dark:bg-gray-700"></div>
              </div>
              <span className="sr-only">Loading...</span>
            </div>
          </div>
        </section>
      </main>
    </div>
  );
}
