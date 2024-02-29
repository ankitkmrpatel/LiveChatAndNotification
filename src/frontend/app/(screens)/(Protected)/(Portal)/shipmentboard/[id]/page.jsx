import React from "react";
import ShipmentLoadingDashboard from "../components/ShipmentLoadingDashboard";
import ShipmentDetailsDashboard from "../components/ShipmentDetailsDashboard";
import Link from "next/link";

export default function ShipmentDetails({ params }) {
  return (
    <>
      <div className="h-full m-2">
        <div className="border-2 rounded-lg border-gray-300 dark:border-gray-600 p-4 mb-4">
          <div className="w-full">
            <Link
              href={"/dashboard"}
              className="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-primary-300 font-medium rounded-lg text-sm px-4 lg:px-5 py-2 lg:py-2.5 mr-2 dark:bg-primary-600 dark:hover:bg-primary-700 focus:outline-none dark:focus:ring-primary-800"
              //className="inline-flex items-center text-white hover:underline bg-blue-500 hover:bg-blue-800 focus:ring-4 "
            >
              Read more
              <svg
                className="w-3.5 h-3.5 ml-2"
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 14 10"
              >
                <path
                  stroke="currentColor"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth="2"
                  d="M1 5h12m0 0L9 1m4 4L9 9"
                />
              </svg>
            </Link>
          </div>
          <React.Suspense fallback={<ShipmentLoadingDashboard />}>
            <ShipmentDetailsDashboard id={params.id} />
          </React.Suspense>
        </div>
      </div>
    </>
  );
}
