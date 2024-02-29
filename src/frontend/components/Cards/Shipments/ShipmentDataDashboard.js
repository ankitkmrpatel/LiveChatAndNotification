import React from "react";
import { headers } from "next/headers";
import Link from "next/link";

async function getData() {
  const res = await fetch("http://localhost:3000/api/shipments/", {
    method: "POST",
    headers: headers(),
    next: { revalidate: 100 },
    body: JSON.stringify({ Message: "" }),
  });

  //console.log("Shipment Request", res);

  if (!res.ok) {
    //throw new Error("Failed to fetch data");
    return [];
  }

  var jsonData = await res.json();
  return jsonData.data;
}

export default async function ShipmentDataDashboard() {
  var data = await getData();
  //console.log(data);

  return (
    <>
      {data.map((shipment, index) => (
        <div
          key={index}
          className="max-w-sm p-6 bg-white border border-gray-200 rounded-lg shadow hover:bg-gray-100 dark:bg-gray-800 dark:border-gray-700 mb-4"
        >
          <div className="flex flex-row justify-between">
            <a href="#">
              <h5 className="mb-2 text-2xl font-bold tracking-tight text-gray-900 dark:text-white">
                {shipment.shipmentId}
              </h5>
            </a>

            <Link
              href={`/dashboard?shipment=${shipment.shipmentId}`}
              className="inline-flex items-center text-blue-600 hover:underline"
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
          <p className="font-normal text-sm text-gray-700 dark:text-gray-400 mb-1">
            Cnee:
            {shipment.cneeCode && ` ${shipment.cneeCode}-${shipment.cneeName}`}
          </p>
          <p className="font-normal text-sm text-gray-700 dark:text-gray-400 mb-1">
            Cnor:
            {shipment.cnorCode && ` ${shipment.cnorCode}-${shipment.cnorName}`}
          </p>
          <p className="font-normal text-sm text-gray-700 dark:text-gray-400 mb-1">
            Goods Desc: {shipment.goodsDesc}
          </p>
          <p className="font-normal text-sm text-gray-700 dark:text-gray-400 mb-1">
            Routing: {shipment.polName} =&gt; {shipment.podName}
          </p>
        </div>
      ))}
    </>
  );
}
